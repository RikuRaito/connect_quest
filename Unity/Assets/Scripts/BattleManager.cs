using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public Animator sordAnimator;
    public Animator enemyAnimator;

    private bool playerTurn = true;

    // HP管理
    public int playerMaxHP = 30;
    public int enemyMaxHP = 30;
    private int playerHP;
    private int enemyHP;

    // 状態
    private bool playerDefending = false;
    private bool enemyDefending = false;

    // UI (HPバー)
    public Slider playerHPSlider;
    public Slider enemyHPSlider;

    // Heal回数制限
    private int playerHealCount = 0;
    private int enemyHealCount = 0;
    private const int maxHealCount = 3;

    // --- 勝敗UI ---
    public GameObject resultPanel;        // 黒背景パネル
    public TextMeshProUGUI resultText;    // 勝敗テキスト

    void Start()
    {
        // HPbar表示
        if (playerHPSlider != null)
        {
            playerHPSlider.maxValue = playerMaxHP;
            playerHPSlider.value = playerHP;
        }
        if (enemyHPSlider != null)
        {
            enemyHPSlider.maxValue = enemyMaxHP;
            enemyHPSlider.value = enemyHP;
        }

        if (resultPanel != null)
        {
            resultPanel.SetActive(false);
        }

        playerHP = playerMaxHP;
        enemyHP = enemyMaxHP;
        Debug.Log($"戦闘開始！ プレイヤーHP:{playerHP}, 敵HP:{enemyHP}");
    }

    // --- プレイヤーコマンド選択 ---
    public void OnAttackButton() => ChooseAction("attack");
    public void OnDefendButton() => ChooseAction("defend");
    public void OnHealButton()   => ChooseAction("heal");

    void ChooseAction(string playerAction)
    {
        if (!playerTurn) return;
        StartCoroutine(ResolveTurn(playerAction));
    }

    // --- ターン解決 ---
    IEnumerator ResolveTurn(string playerAction)
    {
        playerTurn = false;
        playerDefending = false;
        enemyDefending = false;

        // 敵の行動を決定（ランダム）
        string enemyAction = DecideEnemyAction();

        Debug.Log($"プレイヤー:{playerAction} / 敵:{enemyAction}");

        // 先手を判定
        bool playerGoesFirst = DetermineTurnOrder(playerAction, enemyAction);

        if (playerGoesFirst)
        {
            yield return StartCoroutine(ExecuteAction("player", playerAction));
            if (enemyHP > 0 && playerHP > 0)
                yield return StartCoroutine(ExecuteAction("enemy", enemyAction));
        }
        else
        {
            yield return StartCoroutine(ExecuteAction("enemy", enemyAction));
            if (enemyHP > 0 && playerHP > 0)
                yield return StartCoroutine(ExecuteAction("player", playerAction));
        }

        if (playerHP <= 0)
        {
            Debug.Log("プレイヤーは倒れた… Game Over");
            ShowResult("GameOver");
            yield break;
        }
        else if (enemyHP <= 0)
        {
            Debug.Log("敵を倒した！");
            ShowResult("GameClear");
            yield break;
        }

        playerTurn = true;
        Debug.Log("ターン終了 → 次のターン開始");
    }

    void ShowResult(string message)
    {
        Debug.Log($"結果: {message}");
        if (resultPanel != null)
            resultPanel.SetActive(true);
    
        if (resultText != null)
        resultText.text = message;


    }

    // --- 行動の実行 ---
    IEnumerator ExecuteAction(string actor, string action)
    {
        if (actor == "player")
        {
            switch (action)
            {
                case "attack":
                    Debug.Log("Sordの攻撃！");
                    sordAnimator.SetTrigger("Attack");
                    yield return new WaitForSeconds(1.0f);
                    DealDamageToEnemy(5);
                    break;

                case "defend":
                    Debug.Log("Sordは身を守っている！");
                    sordAnimator.SetTrigger("Defence");
                    playerDefending = true;
                    break;

                case "heal":
                    if (playerHealCount < maxHealCount)
                    {
                        int healAmount = 5;
                        playerHP = Mathf.Min(playerHP + healAmount, playerMaxHP);
                        playerHealCount++;
                        Debug.Log($"Sordは回復した！（{playerHealCount}/{maxHealCount}回） HP:{playerHP}/{playerMaxHP}");
                        sordAnimator.SetTrigger("Heal");
                        UpdateHPBars();
                    }
                    else
                    {
                        Debug.Log("Sordはもう回復できない！");
                    }
                    break;
            }
        }
        else if (actor == "enemy")
        {
            switch (action)
            {
                case "attack":
                    Debug.Log("Enemyの攻撃！");
                    enemyAnimator.SetTrigger("Attack");
                    yield return new WaitForSeconds(1.0f);
                    DealDamageToPlayer(5);
                    break;

                case "defend":
                    Debug.Log("Enemyは身を守っている！");
                    enemyAnimator.SetTrigger("Defence");
                    enemyDefending = true;
                    break;

                case "heal":
                    if (enemyHealCount < maxHealCount)
                    {
                        int healAmount = 5;
                        enemyHP = Mathf.Min(enemyHP + healAmount, enemyMaxHP);
                        enemyHealCount++;
                        Debug.Log($"Enemyは回復した！（{enemyHealCount}/{maxHealCount}回） HP:{enemyHP}/{enemyMaxHP}");
                        enemyAnimator.SetTrigger("Heal");
                        UpdateHPBars();
                    }
                    else
                    {
                        Debug.Log("Enemyはもう回復できない！");
                    }
                    break;
            }
        }
    }

    // --- 敵の行動を決定 ---
    string DecideEnemyAction()
    {
        if (enemyHP <= enemyMaxHP / 3 && enemyHealCount < maxHealCount)
        {
            return Random.Range(0, 2) == 0 ? "heal" : "attack";
        }
        else
        {
            int r = Random.Range(0, 3);
            return r == 0 ? "attack" : r == 1 ? "defend" : "heal";
        }
    }

    // --- 行動順判定 ---
    bool DetermineTurnOrder(string playerAction, string enemyAction)
    {
        if (playerAction == "defend" && enemyAction != "defend") return true;
        if (enemyAction == "defend" && playerAction != "defend") return false;
        if (playerAction == "defend" && enemyAction == "defend") return true; // 両方防御ならプレイヤー先
        return true; // それ以外はプレイヤー先
    }

    // --- ダメージ処理 ---
    void DealDamageToEnemy(int amount)
    {
        if (enemyDefending) amount /= 2;
        enemyDefending = false;
        enemyHP -= amount;
        Debug.Log($"Enemyに{amount}ダメージ！ EnemyHP:{enemyHP}/{enemyMaxHP}");
        UpdateHPBars();
    }

    void DealDamageToPlayer(int amount)
    {
        if (playerDefending) amount /= 2;
        playerDefending = false;
        playerHP -= amount;
        Debug.Log($"プレイヤーに{amount}ダメージ！ PlayerHP:{playerHP}/{playerMaxHP}");
        UpdateHPBars();
    }
    // HPバー更新
    void UpdateHPBars()
    {
        if (playerHPSlider != null)
            playerHPSlider.value = playerHP;
        if (enemyHPSlider != null)
            enemyHPSlider.value = enemyHP;
    }
}