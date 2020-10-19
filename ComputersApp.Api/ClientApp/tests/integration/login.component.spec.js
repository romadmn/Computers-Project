/// <reference types="Cypress" />

describe("Login", () => {
  beforeEach(() => {
    cy.clearSession();
  });

  it("should login user", () => {
    cy.visit("/login");
    cy.get('.loginForm')
      .find('[type="text"]').type(Cypress.env('adminEmail')).should('have.value', Cypress.env('adminEmail'))
      cy.get('.loginForm')
      .find('[type="password"]').type(Cypress.env('password')).should('have.value', Cypress.env('password'))
    cy.get('.loginForm').submit().as('login')
    cy.waitFor('@login');
    cy.get('.navbar-collapse').find('ul>li')
      .last().find('a').as('logOutLink').should('be.visible')
    cy.get('.new-pc').find('i').first().should('contain', 'Add new computer')
  });

  it("should not login user", () => {
    cy.visit("/login");
    cy.get('.loginForm')
      .find('[type="text"]').type(Cypress.env('testEmail')).should('have.value', Cypress.env('testEmail'))
      cy.get('.loginForm')
      .find('[type="password"]').type(Cypress.env('password')).should('have.value', Cypress.env('password'))
    cy.get('.loginForm').submit().as('login')
    cy.waitFor('@login');
    cy.get('.loginForm').should('contain', 'Invalid credentials! Please try again!');
  });

  it("should not validate email input", () => {
    const invalidEmail = 'helloworld'
    cy.visit("/login");
    cy.get('.loginForm')
      .find('[type="text"]').type(invalidEmail).should('have.value', invalidEmail)
    cy.get('.loginForm').submit().as('login')
    cy.waitFor('@login');
    cy.get('.loginForm')
      .find('[type="text"]').should('have.class', 'is-invalid');
    cy.get('.loginForm').should('contain', 'Please provide a valid email address');
  });

  it("should not validate password input", () => {
    const invalidPassword = 'password'
    cy.visit("/login");
    cy.get('.loginForm')
      .find('[type="text"]').type(invalidPassword).should('have.value', invalidPassword)
    cy.get('.loginForm')
      .find('[type="password"]').type(Cypress.env('testEmail')).should('have.value', Cypress.env('testEmail'))
    cy.get('.loginForm').submit().as('login')
    cy.waitFor('@login');
    cy.get('.loginForm')
      .find('[type="password"]').should('have.class', 'is-invalid');
    cy.get('.loginForm').should('contain', 'At least one lowercase char, uppercase char and one number');
  });
});