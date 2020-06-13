# Sharesies To Sharesight #
[![Travis build status](https://travis-ci.org/0Lucifer0/SharesiesToSharesight.svg?branch=master)](https://travis-ci.org/0Lucifer0/SharesiesToSharesight)

## Referral Links ##
If you are interested in investing from New Zealand here some referral links:
- <a href="https://sharesies.nz/r/X99G4R"/><img src="https://static1.squarespace.com/static/58bc788c59cc68b9696b9ee0/t/5bfe00514ae23736655bacca/1591327857356/" height="45"/></a><br/>
The referral link offers you $5.00
- <a href="https://portfolio.sharesight.com/refer/CEM81"/><img src="https://www.sharesight.com/img/logos/logo-11a4fd04.svg" height="70"/></a><br/>
The referral link offers you 10% off annual billing
- <a href="https://app.hatchinvest.nz/"/><img src="https://pbs.twimg.com/media/D_jLsLLWkAwhg0p.png" height="50"/></a><br/>
No referral system however it has Sharesight integration!
- <a href="https://hellostake.com/referral-program?referrer=erwanj724"/><img src="https://www.moneyhub.co.nz/uploads/1/1/2/1/112100199/stake-review-trading_1.png?ezimgfmt=rs:350x162/rscb7/ng:webp/ngcb7" height="50"/></a><br/>
The referral link offers you a Gopro, Dropbox or Nike free stock! Sharesight can parse the email as a portfolio email

## How to build ? ##
Ensure you have dotnet core 3.1 installed: https://dotnet.microsoft.com/download
Run script/build.cmd

## How to run it ? ##
Run script/run.cmd

## How to use it ? ##
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
