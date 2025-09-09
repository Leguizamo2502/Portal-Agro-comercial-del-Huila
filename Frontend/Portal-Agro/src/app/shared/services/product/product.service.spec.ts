import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { HttpClient } from '@angular/common/http';

import { ProductService } from './product.service';
import { environment } from '../../../../environments/environment';

describe('ProductService (GETs)', () => {
  let service: ProductService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [ProductService],
    });
    service = TestBed.inject(ProductService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => httpMock.verify());

  it('getAllHome() debe llamar /home SIN query param cuando no se envía limit', () => {
    // Act
    let resp: any[] | undefined;
    service.getAllHome().subscribe(r => (resp = r));

    // Assert
    const req = httpMock.expectOne(request =>
      request.method === 'GET' &&
      request.url === `${environment.apiUrl}Product/home`
    );

    expect(req.request.params.has('limit')).toBeFalse();

    // Respuesta simulada
    req.flush([{ id: 1, name: 'Café' }]);

    expect(resp).toBeTruthy();
    expect(resp!.length).toBe(1);
    expect(resp![0].name).toBe('Café');
  });

  it('getAllHome(10) debe incluir ?limit=10 en la URL', () => {
    service.getAllHome(10).subscribe();

    const req = httpMock.expectOne(request =>
      request.method === 'GET' &&
      request.url === `${environment.apiUrl}Product/home` &&
      request.params.get('limit') === '10'
    );

    expect(req.request.params.get('limit')).toBe('10');
    req.flush([]); // No importa el cuerpo para este test
  });

  // --- Opcionales rápidos para subir cobertura (puedes dejarlos o quitarlos) ---

  it('getFeatured() debe llamar /featured', () => {
    service.getFeatured().subscribe();
    const req = httpMock.expectOne(`${environment.apiUrl}Product/featured`);
    expect(req.request.method).toBe('GET');
    req.flush([]);
  });

  it('getFavorites() debe llamar /favorites', () => {
    service.getFavorites().subscribe();
    const req = httpMock.expectOne(`${environment.apiUrl}Product/favorites`);
    expect(req.request.method).toBe('GET');
    req.flush([]);
  });

  it('getByCategory() debe lanzar error si categoryId <= 0', () => {
    expect(() => service.getByCategory(0)).toThrowError('categoryId inválido');
    expect(() => service.getByCategory(-1)).toThrowError('categoryId inválido');
  });

  it('getByCategory(5) debe llamar /categories/5/products', () => {
    service.getByCategory(5).subscribe();
    const req = httpMock.expectOne(`${environment.apiUrl}Product/categories/5/products`);
    expect(req.request.method).toBe('GET');
    req.flush([]);
  });
});
