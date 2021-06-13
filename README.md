# Checkout.com .NET Challenge
Building a Payment Gateway

---



### Introduction

An API based application that will allow a
merchant to offer away for their shoppers to pay for their product.


1. [How To Build](#How-To-Build)
1. [API Endpoints](#API-Endpoints)
1. [API Entity Schema](#API-Entity-Schema)
1. [Solution Architecure](#Solution -rchitecture)
1. [Containerization](#Containerization)
1. [CI/CD](#CI-/-CD-Pipeline)
1. [API Client](#APi-Client)
1. [Enhancements](#Enhancements)
### Build Status

[![.NET](https://github.com/petefield/checkout/actions/workflows/dotnet.yml/badge.svg)](https://github.com/petefield/checkout/actions/workflows/dotnet.yml) [![Docker Image CI](https://github.com/petefield/checkout/actions/workflows/docker-image.yml/badge.svg)](https://github.com/petefield/checkout/actions/workflows/docker-image.yml)

# How To Build

1. Ensure you have .Net SDK 5 installed.
Dotnet sdks can be downloaded from https://dotnet.microsoft.com/download/visual-studio-sdks.

1. Clone the checkout repository:
`git clone https://github.com/petefield/checkout.git`

1. change to the src dirctory: `cd src`

1. Build the solution: `dotnet build`

1. Run tests `dotnet test`

1. Run the payment gateway api host : `dotnet run --project .\PaymentGateway.Api\PaymentGateway.Api.csproj`  
nb. to change the default url either set the `ASPNETCORE_URLS` environment variable or pass the `--urls` arguement to the run command specifying the desired urls.

1. Point a browser to [https://[host]:[port]/swagger/index.html](https://[host]:[port]/swagger/index.html) to review the API's open Api definition.

# API Endpoints

The PaymentGateway appication exposes the following endpoints

## POST /payments/

This end point can be used to create a new payment request.

### Request Body 

The body of the request should contain a JSON representation of a [Payment Request](#payment-request) entity. This will be validated, and if valid, passed on to the 
acquiring bank.

### Responses

| Status Code | Description |
| ----------- | ----------- |
| 400 - Bad Request| The request body could not be validated. Validation errors are included in the response body.
|201 - Created | The request has been processed. The response body will contain a [Payment Details](#payment-details) entity detailing the result. The response Location Header will contain a url which can be used to obtain the payment details in the future |


## GET /payments/{paymentId}
This end point can be used to rerieve details of a previously submitted payment request.


### Parameters 
| Name | Description |
| ----------- | ----------- |
| {paymentId} | The id of a previusly processed request.  This Id can be obtained by looking at the Request ID property of a [Payment Details](#payment-details) returned via a post to the /payments/ endpoint|.

### Responses

| Status Code | Description |
| ----------- | ----------- |
| 400 - Bad Request| The paymentId parameter could not be parsed as a GUID|
| 404 - Not Found| The paymentId parameter did not represent a payment request previously submitted |
| 200 - OK| The response body will contain a [Payment Details](#payment-details)  entity representing the details of the request with the id represented by the paymentId parameter|

# API Entity Schema

## Expiry date

| name | Description |
| ----------- | ----------- |
| year | An integer representing the year in which the card will expire.|
| month | An integer between 1 & 12 inclusive, representing the month at the end of which the card will expire. |

## Payment Request

| name | Description |
| ----------- | ----------- |
| cardNumber | A string representing a valid card number. This application considers any string that represents a well formed credit card number (ie. one that passes a [Luhn](https://en.wikipedia.org/wiki/Luhn_algorithm) check.) |
| cvv | A string that represents a [CVV](https://en.wikipedia.org/wiki/Card_security_code). To be considered valid, this string must be between 3 & 4 digits long. |
| expiryDate | An [Expiry Date](#expiry-date) that represents the expiry date of the card to be charged. To be considered valid, this date must be in the future.  |
| currencyCode | A valid currency code. Currently only 'GBP' and 'USD' are considered valid codes. |
| amount | The amount to be charged.  This is represented as an integer which will be divided by 100. ie. Passing a value of 1 will result in a charge of 0.01 of the currency code specified. |


## Payment Details
| name | Description |
| ----------- | ----------- |
| id | a unique identifire which may be used to retrieve this payment record in the future.|
| cardNumber | The last 4 digits of the card that was charged. |
| cvv | The CVV code of the card that was charged. |
| expiryDate | the [Expiry Date](#expiry-date) of the card that was charger.  |
| outcome | Represents the outcome of the transaction - Success | Failed | Held |
| amount | The amount that was charged. |
| received | The date and time at which the request was recieved. |
| processed | The date and time at which the request processing was completed. |

# Solution Architecture

## API
The solution consists of a simple asp.net webapi application that consists of one controller `PaymentsController`.  Into this controller, instances of IPaymentStore and an IAcquiringBank are injected.

## Data storage
The IPaymentStore is an interface that defines methods to store and retreive payment request details. In this implementation, and in-memory store is used.

## Acquiring bank.
The IAcquiringBank is an interface that defines methods used to pass a payment request on to an Aquiring Bank.

In this implementation an in-memory instance of an Aquiring Bank is used.  This implementation performs no validation or logic, other than checking the last digit of the CVV code passed to it.

If the CVV code ends in a 0, the payment is considered to have failed due to insufficient funds.
If the CVV code ends in a 1, the payment is considered to have failed due suspected fraud.
If the CVV code ends in any other dgit, the payment is considered succesfull.

# Containerization
A docker file is included at in \src\PaymentGateway.Api.  

To build a docker image using this file :
1. change to the src directory: `cd source`.
1. build an image: `docker build . --file ./PaymentGateway.Api/dockerfile --tag {tagname}`
1. run `docker run -p 8080:80 {imagename}`

# CI / CD Pipeline

This solution uses GitHub Actions as its CI/CD pipeline.  Workflow definitions are in .githib actions folder in the repository root.

## Build and Test

[![.NET](https://github.com/petefield/checkout/actions/workflows/dotnet.yml/badge.svg)](https://github.com/petefield/checkout/actions/workflows/dotnet.yml)

This pipeline uses the Dotnet CLI to build the solution and run all tests.

## Deploy

[![Docker Image CI](https://github.com/petefield/checkout/actions/workflows/docker-image.yml/badge.svg)](https://github.com/petefield/checkout/actions/workflows/docker-image.yml)

This pipleline builds a docker image using the docker file in the ./src folder.  It then pushes that image to [DockerHub](https://hub.docker.com/repository/docker/petefield/checkout).

An Azure App Service instance is configured to pull an image from the above docker hub repository when ever a new image is pushed. It then restarts the application located at https://checkout-paymentgateway.azurewebsites.net/swagger/index.html.

# API Client

There is a class library project (./src/PaymentGateway.Client/PaymentGateway.Client.csproj) included in the solution. This class library encapsulates the functionality of the API, and allows third party developers to make requests to the API using strongly typed modles and receive a stronly typed response.

An example of the usage of the API client is shown in (./src/PaymentGateway.Clients.Console/PaymentGateway.Clients.Console.csproj).

This is a simple console app that makes a payment request and outputs the results.

# Enhancements

## Authentication: 
Calls to the API should be authenticated. This should be implemented by requiring a bearer token to be pre passed in the request.
Consumers of the API would need to generate a bearer token, using a client secret, provided by the host of the api.

## Multi-Tenancy:
Currently the application has no idea who payments are being requested by. Merchants should be required to pass a MerchantID as part of the payment request (or one should be inferred from bearer token.). This could then be used which merchant was calling the API, and how to treat the request. (ie.  different acquiring banks could be used etc.)

## Versioning
Currently clients have no method of using different versions of the API.  If a client wishes to consume a version of the API they should be able to specify the version either on the URL (i.e http://example.com/v1/payments) or via a version header in the request.

## Leaky abstractions
The Client class library currently leaks some abstractions that should not be exposed to the client. A re-implementation of some of the models.  Using Automapper (for example.) to map between domain specific implementations of some of the Model interfaces would be a worthwhile exercise.