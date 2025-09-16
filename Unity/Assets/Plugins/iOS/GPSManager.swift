import UIKit
import CoreLocation

class GPSOverlay: NSObject, CLLocationManagerDelegate {
    static let shared = GPSOverlay()
    private let locationManager = CLLocationManager()
    private var label: UILabel?

    override init() {
        super.init()
        locationManager.delegate = self
        locationManager.requestWhenInUseAuthorization()//アプリ使用中のみ許可の場合
        locationManager.requestAlwaysAuthorization()//常に許可を表示
        locationManager.allowsBackgroundLocationUpdates = true
        locationManager.pausesLocationUpdatesAutomatically = false
    }

    func start() {
        // ラベルを作成して画面に追加
        if let window = UIApplication.shared.windows.first {
            let lbl = UILabel(frame: CGRect(x: 10, y: 40, width: 300, height: 50))
            lbl.textColor = .white
            lbl.backgroundColor = UIColor.black.withAlphaComponent(0.5)
            lbl.font = UIFont.systemFont(ofSize: 14)
            lbl.numberOfLines = 2
            window.addSubview(lbl)
            self.label = lbl
        }
        locationManager.startUpdatingLocation()//位置情報の更新を開始
    }

    func locationManager(_ manager: CLLocationManager, didUpdateLocations locations: [CLLocation]) {
        guard let loc = locations.last else { return }
        print("locationManager")
        let latitude = loc.coordinate.latitude
        let longitude = loc.coordinate.longitude

        DispatchQueue.main.async {
            print("更新： Lat\(latitude), Lng \(longitude)")
            if let lbl = self.label {
                lbl.text = String(format: "Lat: %.12f\nLng: %.12f", latitude, longitude)    
            } else {
                print("ラベルがnilです")
            }
            
        }
    }
}

// Unity から呼び出すエントリポイント
@_cdecl("startGPSOverlay")
public func startGPSOverlay() {
    GPSOverlay.shared.start()
}
