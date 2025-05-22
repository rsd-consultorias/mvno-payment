# Connected Cars API

## Endpoints

### Real-Time Monitoring

#### Get Vehicle Location
**Endpoint:** `/vehicle/{vehicle_id}/location`  
**Method:** `GET`  
**Description:** Retrieves the real-time location of a vehicle.  
**Parameters:**  
- None (the `vehicle_id` in the URL identifies the vehicle).

#### Get Vehicle Status
**Endpoint:** `/vehicle/{vehicle_id}/status`  
**Method:** `GET`  
**Description:** Retrieves the current status of the vehicle, such as fuel level and battery status.  
**Parameters:**  
- None (the `vehicle_id` in the URL identifies the vehicle).

#### Configure Alerts
**Endpoint:** `/vehicle/{vehicle_id}/alerts`  
**Method:** `POST`  
**Description:** Configures custom alerts for the vehicle (e.g., low fuel, maintenance required).  
**Parameters:**  
- `alert_type` (string, required) - Type of alert to configure.
- `threshold` (string, optional) - Threshold value for triggering the alert.

---

### Vehicle Security

#### Lock/Unlock Vehicle
**Endpoint:** `/vehicle/{vehicle_id}/lock`  
**Method:** `POST`  
**Description:** Locks or unlocks the vehicle remotely.  
**Parameters:**  
- `action` (string, required) - Values: `lock`, `unlock`.

#### Send Emergency Alert
**Endpoint:** `/vehicle/{vehicle_id}/emergency-alert`  
**Method:** `POST`  
**Description:** Sends an emergency alert in case of incidents like accidents.  
**Parameters:**  
- `alert_message` (string, optional) - Details about the emergency.

#### Intrusion Detection
**Endpoint:** `/vehicle/{vehicle_id}/intrusion-detection`  
**Method:** `GET`  
**Description:** Checks for past intrusion attempts on the vehicle.  
**Parameters:**  
- None (the `vehicle_id` in the URL identifies the vehicle).

---

### Navigation and Travel Services

#### Get Optimized Routes
**Endpoint:** `/navigation/routes`  
**Method:** `GET`  
**Description:** Retrieves optimized routes based on real-time traffic.  
**Query Parameters:**  
- `origin` (string, required) - Starting point of the route.
- `destination` (string, required) - Ending point of the route.

#### Find Points of Interest
**Endpoint:** `/navigation/points-of-interest`  
**Method:** `GET`  
**Description:** Searches for points of interest near a location.  
**Query Parameters:**  
- `location` (string, required) - The location to search around.
- `type` (string, optional) - Type of points of interest (e.g., `restaurant`, `gas_station`).

#### Get Trip History
**Endpoint:** `/vehicle/{vehicle_id}/trip-history`  
**Method:** `GET`  
**Description:** Retrieves past trips of the vehicle.  
**Parameters:**  
- None (the `vehicle_id` in the URL identifies the vehicle).

---

### Entertainment and Connectivity

#### Configure Wi-Fi
**Endpoint:** `/vehicle/{vehicle_id}/wifi-config`  
**Method:** `POST`  
**Description:** Configures Wi-Fi settings for the vehicle.  
**Parameters:**  
- `ssid` (string, required) - Wi-Fi network name.
- `password` (string, required) - Wi-Fi network password.

#### List Entertainment Options
**Endpoint:** `/vehicle/{vehicle_id}/entertainment`  
**Method:** `GET`  
**Description:** Retrieves available entertainment options in the vehicle.  
**Parameters:**  
- None (the `vehicle_id` in the URL identifies the vehicle).

#### Configure Voice Assistant
**Endpoint:** `/vehicle/{vehicle_id}/voice-assistant`  
**Method:** `POST`  
**Description:** Configures settings for the vehicle's voice assistant.  
**Parameters:**  
- `language` (string, required) - Preferred language for the assistant.
- `voice` (string, optional) - Voice profile settings.

---

### Payments and Expenses

#### Register Toll Payment
**Endpoint:** `/vehicle/{vehicle_id}/payment/toll`  
**Method:** `POST`  
**Description:** Registers automatic toll payments for the vehicle.  
**Parameters:**  
- `toll_id` (string, required) - Identifier for the toll.

#### Register Parking Payment
**Endpoint:** `/vehicle/{vehicle_id}/payment/parking`  
**Method:** `POST`  
**Description:** Registers parking payment for the vehicle.  
**Parameters:**  
- `parking_id` (string, required) - Identifier for the parking location.

#### Get Vehicle Expenses
**Endpoint:** `/vehicle/{vehicle_id}/expenses`  
**Method:** `GET`  
**Description:** Retrieves a list of accumulated expenses for the vehicle.  
**Parameters:**  
- None (the `vehicle_id` in the URL identifies the vehicle).

---

### Subscriptions and Plans

#### Create Subscription
**Endpoint:** `/subscriptions`  
**Method:** `POST`  
**Description:** Creates a subscription for a specific service or package.  
**Parameters:**  
- `user_id` (string, required) - The user subscribing.
- `plan_id` (string, required) - The plan to subscribe to.
- `vehicle_id` (string, optional) - The vehicle associated with the subscription.

#### Get Subscription Details
**Endpoint:** `/subscriptions/{subscription_id}`  
**Method:** `GET`  
**Description:** Retrieves details of a specific subscription.  
**Parameters:**  
- None (the `subscription_id` in the URL identifies the subscription).

#### Update Subscription
**Endpoint:** `/subscriptions/{subscription_id}`  
**Method:** `PUT`  
**Description:** Updates the details of an existing subscription.  
**Parameters:**  
- Any fields from subscription creation can be sent to update the subscription.

#### Delete Subscription
**Endpoint:** `/subscriptions/{subscription_id}`  
**Method:** `DELETE`  
**Description:** Cancels an existing subscription.  
**Parameters:**  
- None (the `subscription_id` in the URL identifies the subscription).

---

### Fleet Management (For Enterprises)

#### List Fleet Vehicles
**Endpoint:** `/fleet/vehicles`  
**Method:** `GET`  
**Description:** Retrieves a list of all vehicles in the fleet.  
**Parameters:**  
- None.

#### Get Vehicle Report
**Endpoint:** `/fleet/{vehicle_id}/report`  
**Method:** `GET`  
**Description:** Generates a detailed report for a specific fleet vehicle.  
**Parameters:**  
- None (the `vehicle_id` in the URL identifies the vehicle).

#### Configure Fleet Settings
**Endpoint:** `/fleet/{vehicle_id}/settings`  
**Method:** `POST`  
**Description:** Configures specific settings for a fleet vehicle.  
**Parameters:**  
- `limit` (string, optional) - Usage limits or restrictions for the vehicle.
- `preferences` (object, optional) - Additional configuration preferences.
