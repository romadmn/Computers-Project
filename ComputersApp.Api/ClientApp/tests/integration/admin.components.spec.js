/// <reference types="Cypress" />

describe("AdminComponent", () => {
  
    it("should not be accessable by users who are not admins", () => {
      cy.clearSession();
      cy.login(Cypress.env('userEmail'), Cypress.env('password'));
      cy.visit("/");
      cy.get('.navbar').should('not.contain', 'Admin panel')
    });

    it("should be accessable by admin", () => {
        cy.clearSession();
        cy.login(Cypress.env('adminEmail'), Cypress.env('password'));
        cy.visit("/");
        cy.get('.navbar').should('contain', 'Admin panel')
        cy.visit("/admin");
        cy.contains("This page is visible only for Admin!");
      });
  });