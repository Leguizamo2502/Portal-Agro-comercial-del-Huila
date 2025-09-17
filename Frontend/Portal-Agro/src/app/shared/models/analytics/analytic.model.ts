
export interface TopProductStat {
  productId: number;
  productName: string;
  completedOrders: number;
  totalUnits: number;
  totalRevenue: number;
}

export interface TopProductsResponse {
  items: TopProductStat[];
  totalProducts: number;
}
