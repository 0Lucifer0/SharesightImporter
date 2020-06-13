# Sharesies To Sharesight #
[![Travis build status](https://travis-ci.org/0Lucifer0/SharesiesToSharesight.svg?branch=master)](https://travis-ci.org/0Lucifer0/SharesiesToSharesight)

## Referal Links ##
If you are interested in investing from New Zealand here some referal links:
- Sharesies : https://sharesies.nz/r/X99G4R ($5.00)
- Sharesight : https://portfolio.sharesight.com/refer/CEM81 (10% of annual billing)
- Hatch : https://app.hatchinvest.nz/ (no referal system sorry however it has Sharesight integration!)
- Stake : https://hellostake.com/referral-program?referrer=erwanj724 (Gopro, Dropbox or Nike free stock! Sharesight can parse the email as a portfolio email)

## How to build ##
Ensure you have dotnet core 3.1 installed: https://dotnet.microsoft.com/download

## How to use it ##
To be able to use this soft you need to ask Sharesight API access. 
then just fill the config file

config.yml
```
SharesiesClient:
  Email: ''
  Password: ''
SharesightClient:
  CliendId: ''
  ClientSecret: ''
  PortfolioId: ''
```
