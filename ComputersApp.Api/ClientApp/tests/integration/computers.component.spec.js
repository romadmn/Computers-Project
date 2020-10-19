/// <reference types="Cypress" />

describe("ComputersComponent", () => {
    beforeEach(() => {
      cy.clearSession();
      cy.login(Cypress.env('adminEmail'), Cypress.env('password'));
      cy.reload();
      cy.visit("/");
      cy.url().should('eq', Cypress.config('baseUrl') + '/')
      cy.get('body').should('not.contain', 'No computers found.');
    });

    it("should add new computer", () => {
      let testPc = { name: "Test Computer", ssdAmount: 256, ramAmount: 16, osType: 'Windows', cpu: 'Ukrainian' }
      cy.get('.new-pc').as('addPcPopUp')
      cy.get('@addPcPopUp').click()
      cy.waitFor('@addPcPopUp')
      cy.get('.form-popup')
      .find('[formControlName="name"]').type(testPc.name).should('have.value', testPc.name)
      cy.get('.form-popup')
      .find('[formControlName="ssdAmount"]').type(testPc.ssdAmount).should('have.value', testPc.ssdAmount)
      cy.get('.form-popup')
      .find('[formControlName="ramAmount"]').type(testPc.ramAmount).should('have.value', testPc.ramAmount)
      cy.get('.form-popup')
      .find('[formControlName="osType"]').select(testPc.osType)
      cy.get('.form-popup')
      .find('[formControlName="cpu"]').select(testPc.cpu)
      cy.get('.form-popup')
      .find('[name="submit"]').click();
      cy.get('.table-striped').find('tbody>tr').should('contain', testPc.name)

    });

    it("should not add new computer if user provide invalid data", () => {
      let testPc = { ssdAmount: 256, ramAmount: 16, osType: 'Windows', cpu: 'Ukrainian' }
      cy.get('.new-pc').as('addPcPopUp')
      cy.get('@addPcPopUp').click()
      cy.waitFor('@addPcPopUp')
      cy.get('.form-popup')
      .find('[formControlName="ssdAmount"]').type(testPc.ssdAmount).should('have.value', testPc.ssdAmount)
      cy.get('.form-popup')
      .find('[formControlName="ramAmount"]').type(testPc.ramAmount).should('have.value', testPc.ramAmount)
      cy.get('.form-popup')
      .find('[formControlName="osType"]').select(testPc.osType)
      cy.get('.form-popup')
      .find('[formControlName="cpu"]').select(testPc.cpu)
      cy.get('.form-popup')
      .find('[name="submit"]').click().as('adding');
      cy.waitFor('@adding')
      cy.get('.form-popup')
      .find('[formControlName="name"]').should('have.class', 'is-invalid');
      cy.get('.form-popup').should('contain', 'Name is required');
      cy.get('.table-striped').find('tbody>tr').should('not.contain', testPc.name)

    });

    it("should edit computer", () => {
      var newName = 'Test computerEdited'
      cy.get('.table-striped').find('tbody>tr')
      .last().find('td>button').first().as('editBtn')
      cy.get('@editBtn').click()
      cy.waitFor('@addPcPopUp')
      cy.get('.form-popup')
      .find('[formControlName="name"]').clear().type(newName).should('have.value', newName)
      cy.get('.form-popup').submit();
      cy.get('.table-striped').find('tbody>tr').should('contain', newName)
    });

    it("should not edit computer if user provide invalid data", () => {
      cy.get('.table-striped').find('tbody>tr')
      .last().find('td>button').first().as('editBtn')
      cy.get('@editBtn').click()
      cy.waitFor('@addPcPopUp')
      cy.get('.form-popup')
      .find('[formControlName="name"]').clear()
      cy.get('.form-popup').submit().as('edit');
      cy.waitFor('@edit')
      cy.get('.form-popup')
      .find('[formControlName="name"]').should('have.class', 'is-invalid');
      cy.get('.form-popup').should('contain', 'Name is required');
    });

    it("should remove computer", () => {
      cy.get('.table-striped').find('tbody>tr')
      .last().find('td>button').last().as('deleteBtn')
      cy.get('@deleteBtn').click()
    });
  });