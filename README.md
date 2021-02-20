# Sharesight Importer #
[![Travis build status](https://travis-ci.com/0Lucifer0/SharesightImporter.svg?branch=master)](https://travis-ci.com/0Lucifer0/SharesightImporter)<br/>
This tool automatically add all of your trades into a Sharesight portfolio.

## Referral Links ##
If you are interested in investing from New Zealand here some useful referral links:
- <a href="https://sharesies.nz/r/X99G4R"/><img src="https://static1.squarespace.com/static/58bc788c59cc68b9696b9ee0/t/5bfe00514ae23736655bacca/1591327857356/" height="45"/></a><br/>
The referral link offers you $5.00
- <a href="https://portfolio.sharesight.com/refer/CEM81"/><img src="https://www.sharesight.com/img/logos/logo-11a4fd04.svg" height="70"/></a><br/>
The referral link offers you 10% off annual billing
- <a href="https://app.hatchinvest.nz/share/rbfyt2dm"/><img src="https://pbs.twimg.com/media/D_jLsLLWkAwhg0p.png" height="50"/></a><br/>
$10 when depositing $100 or more
- <a href="https://hellostake.com/referral-program?referrer=erwanj724"/><img src="https://www.moneyhub.co.nz/uploads/1/1/2/1/112100199/stake-review-trading_1.png?ezimgfmt=rs:350x162/rscb7/ng:webp/ngcb7" height="50"/></a><br/>
The referral link offers you a Gopro, Dropbox or Nike free stock! Sharesight can parse the email as a portfolio email

## What is currently supported ? ##
Ethereum wallet (Etherscan api)
Sharesies wallet
Csv file

## How to build ? ##
Ensure you have dotnet 5 installed: https://dotnet.microsoft.com/download
> scripts/build.cmd

## How to run it ? ##
> scripts/run.cmd

## Can I run it as a background task ? ##
Yes you can! 
> scripts/setup-service.cmd

(this has to be ran as Administrator)
It will automatically setup this tool as a windows service. 
The Service will then automatically sync payments every one hour.

## How to use it ? ##
To be able to use this soft you need to ask Sharesight API access. 
then just fill the config file with at least one Exporter

config.yml
```

Exporters:
- ExporterType: Sharesies
  Email: ''
  Password: ''
  PortfolioId: ''
- ExporterType: Ethereum
  EtherscanApiKey: ''
  Addresses:
  - ''
  PortfolioId: ''
- ExporterType: Csv
  Path: 'import.csv'
  PortfolioId: ''
Importers:
- ImporterType: Sharesight
  CliendId: ''
  ClientSecret: ''
- ImporterType: Csv
  Path: 'export.csv'
```
