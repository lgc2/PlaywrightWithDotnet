Feature: Product
Create a new product

Background:
	Given I ensure "Headphones" data is cleaned up if it already exists

Scenario: Create product and verify the details
	Given I access the create product page
	And I create a product with the following details
		| Name       | Description        | Price | ProductType |
		| Headphones | Noise cancellation | 300   | PERIPHARALS |
	When I click the details link of the newly created product
	Then the I see that all the product details are created as expected