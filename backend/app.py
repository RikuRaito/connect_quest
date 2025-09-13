from flask import Flask, request, jsonify
import datetime

app = Flask(__name__)

# --- ヘルスチェック ---
@app.route("/health", methods=["GET"])
def health():
    return jsonify({
        "status": "ok",
        "time": datetime.datetime.now().isoformat()
    })

# --- 位置情報送信 ---
@app.route("/api/location", methods=["POST"])
def location():
    data = request.get_json(silent=True) or {}
    user_id = data.get("userID")
    latitude = data.get("latitude")
    longitude = data.get("longitude")

    # ログに出力（動作確認用）
    print(f"[{datetime.datetime.now()}] userID={user_id}, lat={latitude}, lon={longitude}")

    return jsonify({"status": "OK"}), 200

if __name__ == "__main__":
    # ポート8765で待ち受け
    app.run(host="0.0.0.0", port=8765, debug=True)
