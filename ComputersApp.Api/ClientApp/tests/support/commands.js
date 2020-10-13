Cypress.Commands.add('login', (email, password) => {
    cy.request({
      method: 'POST',
      url: '/api/User/authenticate',
      body: {
        email: email,
        password: password
      }
    }).then((response) => {
      localStorage.setItem('currentUser', JSON.stringify(response.body));
    });
  });

  Cypress.Commands.add('deleteUser', (id) => {
    cy.request({
      method: 'DELETE',
      url: `/api/User/${id}`,
    }).then((response) => {
      localStorage.removeItem('currentUser');
    });
  });

  Cypress.Commands.add('getUserId', () => {
    let user = localStorage.getItem('currentUser');
    var obj = JSON.parse(user)
    return cy.wrap(obj.id);
  });

Cypress.Commands.add('clearSession', () => {
    localStorage.removeItem('currentUser');
});