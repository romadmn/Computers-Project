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
});