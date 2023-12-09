# Minimal.Api.Net8

![enter image description here](/docs/swagger.png)
## AuthEndpoints

- ### SignIn

	 Request
	```json
	{
	  "userName": "string",
	  "password": "string"
	}
	```
	Response
	```json
	{
	  "isSuccess": true,
	  "correlationId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
	  "result": {
	    "user": {
	      "id": 0,
	      "name": "string",
	      "userName": "string"
	    },
	    "token": "string"
	  },
	  "statusCode": 100,
	  "status": "string",
	  "errors": [
	    "string"
	  ]
	}
	```
- ### SignUp

	 Request
	```json
	{
	  "userName": "string",
	  "name": "string",
	  "password": "string"
	}
	```
	Response
	```json
	{
	  "isSuccess": true,
	  "correlationId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
	  "result": {
	    "id": 0,
	    "name": "string",
	    "userName": "string"
	  },
	  "statusCode": 100,
	  "status": "string",
	  "errors": [
	    "string"
	  ]
	}
	```
## CouponEndpoints

- ### GetAllCoupons

	Header
	```json
	"Authorization" : "Bearer token"
	```
	Response
	```json
	{
	  "isSuccess": true,
	  "correlationId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
	  "result": [
	    {
	      "id": 0,
	      "name": "string",
	      "percent": 0,
	      "isActive": "string"
	    }
	  ],
	  "statusCode": 100,
	  "status": "string",
	  "errors": [
	    "string"
	  ]
	}
	```	
	
- ### CreateCoupon

	Header
	```json
	"Authorization" : "Bearer token"
	```
	Request
	```json
	{
	  "name": "string",
	  "percent": 0,
	  "isActive": "string"
	}
	```

	Response
	```json
	{
	  "isSuccess": true,
	  "correlationId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
	  "result": {
	    "id": 0,
	    "name": "string",
	    "percent": 0,
	    "isActive": "string"
	  },
	  "statusCode": 100,
	  "status": "string",
	  "errors": [
	    "string"
	  ]
	}
	```
- ### GetCoupon

	Header
	```json
	"Authorization" : "Bearer token"
	```
	Param 
	```csharp
	string id
	```
   Response
	```json
	{
	  "isSuccess": true,
	  "correlationId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
	  "result": {
	    "id": 0,
	    "name": "string",
	    "percent": 0,
	    "isActive": "string"
	  },
	  "statusCode": 100,
	  "status": "string",
	  "errors": [
	    "string"
	  ]
	}
	```
- ### UpdateCoupon

	Header
	```json
	"Authorization" : "Bearer token"
	```
	Param 
	```csharp
	string id
	```
   Request
	```json
	{
	  "name": "string",
	  "percent": 0,
	  "isActive": "string"
	}
	```   
   Response
	```json
	{
	  "isSuccess": true,
	  "correlationId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
	  "result": {
	    "id": 0,
	    "name": "string",
	    "percent": 0,
	    "isActive": "string"
	  },
	  "statusCode": 100,
	  "status": "string",
	  "errors": [
	    "string"
	  ]
	}
	```
- ### DeleteCoupon

	Header
	```json
	"Authorization" : "Bearer token"
	```
	Param 
	```csharp
	string id
	```
   Response
	```json
	{
	  "isSuccess": true,
	  "correlationId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
	  "result": {
	    "id": 0,
	    "name": "string",
	    "percent": 0,
	    "isActive": "string"
	  },
	  "statusCode": 100,
	  "status": "string",
	  "errors": [
	    "string"
	  ]
	}
	```	
- ### SearchCoupon

	Header
	```json
	"Authorization" : "Bearer token"
	```
	Param 
	```csharp
	string CouponName (query)
	int PageSize (header)
	int Page (header)
	```
   Response
	```json
	{
	  "isSuccess": true,
	  "correlationId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
	  "result": [
	    {
	      "id": 0,
	      "name": "string",
	      "percent": 0,
	      "isActive": "string"
	    }
	  ],
	  "statusCode": 100,
	  "status": "string",
	  "errors": [
	    "string"
	  ]
	}
	```