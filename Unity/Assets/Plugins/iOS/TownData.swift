import Foundation
import UIKit

@_silgen_name("UnitySendMessage")
func UnitySendMessage(_ obj: UnsafePointer<CChar>, _ method: UnsafePointer<CChar>, _ msg: UnsafePointer<CChar>)

@objc class TownData: NSObject {
    @objc static let shared = TownData()

    private var facilityData: [String: Int] = [
        "training": 1,
        "school": 2,
        "restaurant": 0,
        "inn": 1,
        "gym": 0,
        "farm": 3,
        "blacksmith": 2
    ]

    func sendFacilityData() {
        print("Called by FacilityManager")
        if let jsonData = try? JSONSerialization.data(withJSONObject: facilityData, options: []),
           let jsonString = String(data: jsonData, encoding: .utf8) {
            UnitySendMessage("FacilityManager", "ReceiveFacilityData", jsonString)
        }
    }

    func upgradeFacility() {
        //ここでサーバーにアップグレードに関する情報を送信する
    }
}

@_cdecl("sendFacilityData")
public func sendFacilityData() {
    TownData.shared.sendFacilityData()
}

@_cdecl("Appgrade")
public func testAppgrade() {
    TownData.shared.upgradeFacility()
}

