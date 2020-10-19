import {TestBed} from '@angular/core/testing';
import {HttpClientTestingModule, HttpTestingController} from '@angular/common/http/testing';
import {HTTP_INTERCEPTORS, HttpClient} from '@angular/common/http';
import {JwtInterceptor} from './jwt.interceptor';
import { CpuService } from '../services/cpu.service';

xdescribe('JwtInterceptor', () => {
  // Arrange
  let httpClient: HttpClient;
  let httpTestingController: HttpTestingController;
  let service: CpuService;
  const API_URL = 'https://computersweb.azurewebsites.net/api/cpu/1';
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        CpuService,
        {
          provide: HTTP_INTERCEPTORS,
          useClass: JwtInterceptor,
          multi: true
        }]
    });
    httpClient = TestBed.get(HttpClient);
    httpTestingController = TestBed.get(HttpTestingController);
    service = TestBed.get(CpuService);

let store = {};
const mockLocalStorage = {
  getItem: (key: string): any => {
    return key in store ? store[key] : null;
  },
  setItem: (key: string, value: any) => {
    store[key] = `${value}`;
  },
  removeItem: (key: string) => {
    delete store[key];
  },
  clear: () => {
    store = {};
  }
};

spyOn(localStorage, 'getItem').and.callFake(mockLocalStorage.getItem);
spyOn(localStorage, 'setItem').and.callFake(mockLocalStorage.setItem);
spyOn(localStorage, 'removeItem').and.callFake(mockLocalStorage.removeItem);
spyOn(localStorage, 'clear').and.callFake(mockLocalStorage.clear);
});



  afterEach(() => {
    httpTestingController.verify();
  });



  describe('making http calls', () => {
    it('adds authorization header', () => {
      // Arrange
      const token = '{"id":1,"email":"ferencrman@gmail.com","token":{"jwt":"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjEiLCJlbWFpbCI6ImZlcmVuY3JtYW5AZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJleHAiOjE2MDMxOTM5MzMsImlzcyI6IkNvbXB1dGVycyBBcHAiLCJhdWQiOiJDb21wdXRlcnMgQXBwIn0.yI0TIekeWqF5muO3219X57dxZJ6kG9KKzcfXuJr7uik","refreshToken":"eaFKs3004+N8zj0jUifO1sRAM++y8RF9YJxDdd36ahU="}}';
      localStorage.setItem('currentUser', token);
      service.getById(1).subscribe(response => {
        expect(response).toBeTruthy();
      });
  const authToken = localStorage.getItem('currentUser');
  const req = httpTestingController.expectOne({ method: 'GET', url: API_URL });

  // Assert
  expect(req.request.headers.has('Authorization')).toEqual(true);
  expect(req.request.headers.get('Authorization')).toEqual(`Bearer ${authToken}`);
  });
 });

});
