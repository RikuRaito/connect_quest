import Foundation


@objc public class HTTPManager: NSObject, URLSessionDelegate,  @unchecked Sendable{
    @objc public static let shared = HTTPManager()
    private override init() {}
    static let url = "http://localhost:8765"
    
    // バックグラウンド用のセッション識別子
    private let sessionIdentifier = "com.example.app.backgroundSession"
    
    // URLSession を作成
    lazy var session: URLSession = {
        let config = URLSessionConfiguration.background(withIdentifier: sessionIdentifier)
        config.sessionSendsLaunchEvents = true
        config.isDiscretionary = false
        return URLSession(configuration: config, delegate: self, delegateQueue: nil)
    }()
    
    ///ヘルスチェック
    func healthCheck() {
        guard let healthUrl = URL(string: HTTPManager.url + "/api/health") else {
            UnitySendMessage("GameManager", "OnHealthCheckResult", "❌ Invalid URL")
            return
        }
        
        let task = session.dataTask(with: healthUrl) { data, response, error in
            if let error = error {
                UnitySendMessage("GameManager", "OnHealthCheckResult", "❌ Error: \(error.localizedDescription)")
                return
            }
            if let data = data, let result = String(data: data, encoding: .utf8) {
                UnitySendMessage("GameManager", "OnHealthCheckResult", result)
            } else {
                UnitySendMessage("GameManager", "OnHealthCheckResult", "❌ Decode error")
            }
        }
        task.resume()
    }
    
    
    func sendLocation(lat: Double, lon: Double) {
        guard let url = URL(string: HTTPManager.url + "/api/location") else {return}
        var request = URLRequest(url: url)
        request.httpMethod = "POST"
        request.addValue("application/json", forHTTPHeaderField: "Content-Type")
        
        let body: [String: Any] = [
            "lat": lat,
            "lon": lon
        ]
        guard let jsonData = try? JSONSerialization.data(withJSONObject: body) else {return}
        request.httpBody = jsonData
        
        let task = session.uploadTask(with: request, from: jsonData)
        task.resume()
    }
}

@_cdecl("HealthCheckFromUnity")
public func HealthCheckFromUnity() {
    HTTPManager.shared.healthCheck()
}

@_cdecl("SendLocationFromUnity")
public func SendLocationFromUnity(lat: Double, lon: Double) {
    HTTPManager.shared.sendLocation(lat: lat, lon: lon)
}
