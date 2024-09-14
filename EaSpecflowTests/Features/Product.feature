Feature: Product
Create a new product

Background:
	Given I ensure "Headphoness" data is cleaned up if it already exists
	And I ensure "Mouse XPTO" data is cleaned up if it already exists

Scenario: Create product and verify the details
	Given I access the create product page
	And I create a product with the following details
		| Name        | Description        | Price | ProductType |
		| Headphoness | Noise cancellation | 300   | PERIPHARALS |
	When I click the details link of the newly created product
	Then I see that all the product details are created as expected

Scenario: Check the products count increments once added
	Given I ensure to count the total number of products from DB
	And I access the create product page
	When I create a product with the following details
		| Name       | Description | Price | ProductType |
		| Mouse XPTO | Super mouse | 300   | PERIPHARALS |
	Then I see the count of products increments