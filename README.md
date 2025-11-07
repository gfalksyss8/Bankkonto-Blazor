# Bankkonto-Blazor

A basic Blazor webpage written in C# that emulates a Bank's GUI for displaying and managing a users bank accounts.
Made for _Objektorienterad programmering grund, Inlämningsuppgift Bankkonto (Blazor WASM)_

To download project:
`git clone gfalksyss8/Bankkonto-Blazor`

> [!IMPORTANT]
> Ensure you are using [].NET 8.0.414](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) or newer

Run program from compiler of your choice, opens in default web browser

## Features

- Home
	- Password input that hides all GUI until authorized
	- NAV Menu for pages
	- Import and export of localStorage .json file
		- Button for import is disabled until a file is uploaded
		- Button for export is disabled until user is logged in

- Create account
	- Create a new bankaccount, with validation for incorrect entries and required fields
	
- Manage accounts
	- See all bank accounts created, with all info listed
	- Remove account
	- Emulate days passed for interest calculaton, deposit pending interest to savings account
		- Save interest deposit information in History
	- Change login password, save in localStorage
	
- Deposit & Withdraw
	- Choose an account to deposit or withdraw balance from
		- Validation for incorrect transfers or withdrawals above account total 
		- Saves changes in balance in History
		
- Transaction
	- Transfer balance between two local bank accounts on one account
		- Validation to ensure no transactions between same accounts
			- Warning label
		- Set currency exchange determines amount to send
			- Warning labels when transferring between accounts with differing currency
			
- History 
	- See all transaction history between accounts (Transactions) and external sources (Deposits, withdrawals, interest)
		- Transaction types differ between transfers from and to
		- Displays amount sent, and total amount after balance
	- Sorting of transactions using date and amount, with toggle between ascending and descending