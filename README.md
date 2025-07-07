# Asset Tracker

## Description

This is a .NET 9.0 console application that can be used to track assets for a company.

## Features
- Add new assets via the console

- Automatically convert price from USD to the local currency based on the office location

- Exchange rates fetched live from an API

- Supported office locations:
    - Boston (USD)
    - London (GBP)
    - Tokyo (JPY)
    - Berlin (EUR)
    - Toronto (CAD)
    - Malmö (SEK)

- Lists and sorts assets by 1) office location 2) purchase date

- The assets are color coded based on their expiration date:
    - Red with a cross symbol: Expired
    - Red: Expiring within 3 months
    - Yellow: Expiring within 6 months
- Sample data is included for demonstration purposes

- Assets are saved to and fetched from a database using Entity Framework

## Usage
- When running, the program will show the full list of assets (sample data)
- The program will prompt you to do one of four things: 
    - Add a new asset
    - Update an existing asset
    - Delete an asset
    - Exit the program

- If you choose to add a new asset, you will be prompted to enter:
    - Type (e.g. Laptop, Smartphone etc.)
    - Brand
    - Model
    - Office location
    - Price in USD
    - Purchase date

- If you choose to update an existing asset, you will be prompted to enter
a new value for each property, or leave it blank to keep the current value. 

- If you choose to delete an asset, you will be asked to specify which asset to 
delete by entering the corresponding id. 

- The program automatically:
    - Maps the office location to the correct currency
    - Converts the price from USD to the local currency using live exchange rates
    - Calculates the expiration date (3 years from purchase date)

- After adding/updating/deleting an asset, the program will display the updated list of assets, sorted by office location and purchase date

## Technologies used
- Net 9.0
- C#
- REST API
- Microsoft Entity Framework