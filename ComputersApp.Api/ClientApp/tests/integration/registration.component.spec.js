/// <reference types="Cypress" />

describe("Registration", () => {
    beforeEach(() => {
      cy.clearSession();
    });
  
    it("should register user and then delete", () => {
      cy.visit("/login");
      cy.get('.navbar-collapse').find('ul>li')
        .first().next().find('a').click().as('registrationLink')
      cy.waitFor('@registrationLink');
      cy.get('.form-popup')
      .find('[type="text"]').type(Cypress.env('testEmail')).should('have.value', Cypress.env('testEmail'))
      cy.get('.form-popup')
      .find('[type="password"]').first().type(Cypress.env('password')).should('have.value', Cypress.env('password'))
      cy.get('.form-popup')
      .find('[type="password"]').last().type(Cypress.env('password')).should('have.value', Cypress.env('password'))
      cy.get('.form-popup').submit();
      cy.reload();
      cy.get('.loginForm')
      .find('[type="text"]').type(Cypress.env('testEmail')).should('have.value', Cypress.env('testEmail'))
      cy.get('.loginForm')
      .find('[type="password"]').type(Cypress.env('password')).should('have.value', Cypress.env('password'))
      cy.get('.loginForm').submit().as('login')
      cy.waitFor('@login');
      cy.getUserId().then(value => cy.deleteUser(value));
      cy.reload()
      cy.get('.loginForm')
      .find('[type="text"]').type(Cypress.env('testEmail')).should('have.value', Cypress.env('testEmail'))
      cy.get('.loginForm')
      .find('[type="password"]').type(Cypress.env('password')).should('have.value', Cypress.env('password'))
      cy.get('.loginForm').submit().as('login')
      cy.waitFor('@login');
      cy.get('.loginForm').should('contain', 'Invalid credentials! Please try again!');
    });
  });