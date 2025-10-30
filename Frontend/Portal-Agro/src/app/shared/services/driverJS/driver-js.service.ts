import { Injectable, inject } from '@angular/core';
import { Router } from '@angular/router';
import { driver, DriveStep } from 'driver.js';
import 'driver.js/dist/driver.css';

@Injectable({
  providedIn: 'root'
})
export class DriverJsService {
  private router = inject(Router);
  private openedElements: HTMLElement[] = [];
  private originalStates: Map<HTMLElement, boolean> = new Map();
  private driverInstance: any;

  /**
   * JSON centralizado de tours para cada ruta
   */
  private tours: Record<string, DriveStep[]> = {
    '/home': [
      { element: '#carousel', popover: { title: 'Carrusel', description: 'Aquí se muestran los banners.', side: 'bottom' as const } },
      { element: '#latest-additions', popover: { title: 'Últimos Agregados', description: 'Productos agregados recientemente.', side: 'top' as const } },
      { element: '#featured-products', popover: { title: 'Productos Destacados', description: 'Nuestros productos más recomendados.', side: 'top' as const } }
    ],
    '/home/product': [
      { element: '#filters', popover: { title: 'Filtros', description: 'Usa los filtros para buscar productos por nombre o productor.', side: 'right' as const } },
      { element: '#category-select', popover: { title: 'Categorías', description: 'Selecciona la categoría de productos que deseas ver.', side: 'bottom' as const } },
      { element: '#List-Products', popover: { title: 'Listado de productos', description: 'Aquí aparecen los productos disponibles según tus filtros.', side: 'left' as const } },
      { element: '#pagination', popover: { title: 'Paginación', description: 'Aquí podrás ver la siguiente sección de los demás productos.', align: 'end', side: 'right' as const } }
    ],
    '/account/info': [
      { element: '#Info-basic', popover: { title: 'Información Básica', description: 'Aquí se mostrará la información básica que registraste.', side: 'top' as const } },
      { element: '#actions-S', popover: { title: 'Acciones', description: 'Desde aquí puedes actualizar tus datos o cambiar la contraseña.', side: 'left', align: 'start' } },
      { element: '#update-Account-btn', popover: { title: 'Actualizar Información', description: 'Ahí podrás actualizar tus datos personales.', side: 'top' as const } },
      { element: '#changePassword-btn', popover: { title: 'Cambiar Contraseña', description: 'Ahí podrás cambiar la contraseña.', side: 'top' as const } }
    ],
    '/account/info/updateDataBasic': [
      { element: '#Info-Account-Update', popover: { title: 'Actualizar Datos Básicos', description: 'Aquí podrás modificar tus datos personales.', side: 'top', align: 'start' } },
      { element: '#firstName-field', popover: { title: 'Nombres', description: 'Ingresa tus nombres tal como aparecen en tu documento de identidad.', side: 'right' } },
      { element: '#lastName-field', popover: { title: 'Apellidos', description: 'Ingresa tus apellidos completos.', side: 'right' } },
      { element: '#phoneNumber-field', popover: { title: 'Teléfono', description: 'Introduce tu número de teléfono (10 dígitos).', side: 'right' } },
      { element: '#department-field', popover: { title: 'Departamento', description: 'Selecciona el departamento donde vives.', side: 'right' } },
      { element: '#city-field', popover: { title: 'Ciudad', description: 'Selecciona la ciudad correspondiente al departamento elegido.', side: 'right' } },
      { element: '#address-field', popover: { title: 'Dirección', description: 'Ingresa tu dirección exacta.', side: 'top' } },
      { element: '#actions', popover: { title: 'Acciones', description: 'Acá podrás guardar o cancelar los cambios.', side: 'top' } }
    ],
    '/account/info/changePassword': [
      { element: '#title-form', popover: { title: 'Formulario de cambio de contraseña', description: 'Aquí puedes actualizar tu contraseña.', side: 'bottom' } },
      { element: '#current-password-field', popover: { title: 'Contraseña actual', description: 'Debes ingresar tu contraseña actual.', side: 'right' } },
      { element: '#new-password-field', popover: { title: 'Nueva contraseña', description: 'Ingresa tu nueva contraseña cumpliendo las reglas.', side: 'right' } },
      { element: '#confirm-password-field', popover: { title: 'Confirmar contraseña', description: 'Repite la nueva contraseña.', side: 'right' } },
      { element: '#form-actions', popover: { title: 'Acciones', description: 'Guarda o cancela los cambios.', side: 'top' } }
    ],
    '/account/favorite': [
      { element: '#favorite-title', popover: { title: 'Sección de favoritos', description: 'Aquí verás los productos marcados como favoritos.', side: 'bottom' } },
      { element: '#favorite-list', popover: { title: 'Lista de productos favoritos', description: 'Tus productos favoritos aparecerán aquí.', side: 'right' } },
      { element: '#favorite-empty', popover: { title: 'Sin productos favoritos', description: 'Si aún no tienes productos, verás este mensaje.', side: 'top' } }
    ],
    '/account/orders/:code': [
      { element: '#order-header', popover: { title: 'Detalle del Pedido', description: 'Información completa del pedido.', side: 'bottom' } },
      { element: '#order-summary', popover: { title: 'Resumen', description: 'Producto, precio, cantidad y total.', side: 'right' } },
      { element: '#order-payment', popover: { title: 'Comprobante de Pago', description: 'Sube o revisa el comprobante.', side: 'top' } },
      { element: '#order-actions', popover: { title: 'Confirmar recepción', description: 'Confirma la recepción del pedido.', side: 'top' } }
    ],
    '/account/producer/summary':[
      {element:'#producer-layout-summary', popover:{ title: 'Panel principal', description: 'Aquí podrás visualizar un resumen general y gestionar los pedidos realizados por tus clientes.', side:'over'}},
      {element:'#nav-smy-mang', popover:{ title: 'barra de navegación', description: 'Usa esta barra para moverte entre el resumen, la gestión de productos y tus fincas.', side:'bottom'}},
      {element:'#mat-tab-link-0', popover:{ title: 'Resumen', description: 'Consulta el estado de tus pedidos y accede fácilmente a la información de tu perfil.', side:'top'}},
      {element:'#mat-tab-link-1', popover:{ title: 'Gestión', description: 'Administra tus productos y fincas: edita, agrega o elimina lo que necesites con facilidad.', side:'top'}},
      {element:'#show-my-profile', popover:{ title: 'Ver perfil', description: 'Visualiza la información de tu perfil de productor y revisa tus datos personales.', side:'top'}},
      {element:'#update-my-profile', popover:{ title: 'Actualizar perfil', description: 'Modifica tus datos personales o información de contacto cuando lo necesites.', side:'top'}},
      {element:'#totalOrder', popover:{title:'total de pedidos', description:'Consulta todos los pedidos de tus clientes y revisa cuáles están pendientes o completados.',side:'top'}},
      {element:'#pendings', popover:{title:'Pendientes', description:'Gestiona los pedidos que aún no han sido completados y realiza el seguimiento correspondiente.',side:'top'}},
      {element:'#dashboard', popover:{title:'Productos más vendidos', description:'Visualiza los productos con mayor demanda y analiza cuáles son los más populares entre tus clientes.',side:'top'}},
      {element:'#dashboard-table', popover:{title:'Tabla de estadísticas', description:'Observa de forma gráfica la cantidad promedio de productos vendidos en tus pedidos.',side:'top'}},
    ],
    '/account/producer/management/product':[
      {element:'#nav-smy-mang', popover:{ title: 'barra de navegación', description: 'Usa esta barra para moverte entre el resumen, la gestión de productos y tus fincas.', side:'bottom'}},
      {element:'#mat-tab-link-0', popover:{ title: 'Resumen', description: 'Consulta el estado de tus pedidos y accede fácilmente a la información de tu perfil.', side:'top'}},
      {element:'#mat-tab-link-1', popover:{ title: 'Gestión', description: 'Administra tus productos y fincas: edita, agrega o elimina lo que necesites con facilidad.', side:'top'}},
    ]
  };

  // Obtiene los pasos del tour según la ruta actual
  private getSteps(): DriveStep[] | null {
    const currentUrl = this.router.url.split('?')[0];
    let steps = this.tours[currentUrl];

    if (!steps && currentUrl.startsWith('/account/orders/')) {
      steps = this.tours['/account/orders/:code'];
    }

    return steps || null;
  }

  // Inicia el tour con los pasos de la ruta actual o los que se pasen
  startTour(steps?: DriveStep[]) {
    const tourSteps = steps || this.getSteps();
    if (!tourSteps || !tourSteps.length) {
      console.warn(' No hay pasos definidos para el tour.');
      return;
    }

    this.driverInstance = driver({
      showProgress: true,
      showButtons: ['next', 'previous', 'close'],
      nextBtnText: 'Siguiente',
      prevBtnText: 'Anterior',
      doneBtnText: 'Finalizar',
      steps: tourSteps,
    });

    this.driverInstance.drive();
  }
}
