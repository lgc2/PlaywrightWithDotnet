Feature: OtherFeature
Create a new product

Background:
	Given I ensure "Keyboard" data is cleaned up if it already exists

Scenario: Create product and verify the details (2)
	Given I access the create product page
	And I create a product with the following details
		| Name     | Description        | Price | ProductType |
		| Keyboard | Noise cancellation | 300   | PERIPHARALS |
	When I click the details link of the newly created product
	Then the I see that all the product details are created as expected