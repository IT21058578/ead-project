export interface Review {
    id: string;
    vendorId: string;
    productId: string;
    userId: string;
    message: string;
    rating: number;
}

export interface Product {
  id: string;
  vendorId: string;
  name: string;
  description: string;
  category: string;
  price: number;
  isActive?: boolean;
  countInStock: number;
  lowStockThreshold: number;
  rating?: number;
  imageUrl: string;
}

export interface Order {
    id: string;
    userId: string;
    vendorId: string;
    products: {
      [productId: string]: {
        vendorId: string;
        quantity: number;
        name: string;
        price: number;
      };
    };
    deliveryNote: string;
    deliveryAddress: string;
    deliveryDate: string;
    status?: string;
}

export interface Notification {
  id: string;
  recipient: UserRole[];
  type: string;
  userId: string;
  orderId?: string;
  productId?: string;
  reason: string;
  status: string;
  addresedBy?: string;
}

export interface UserType {
  id: string;
  firstName?: string;
  lastName?: string;
  email?: string;
  password?: string;
  role?: UserRole[];
  isVerified?: boolean;
  isApproved?: boolean;
  rating?: number;
}

type UserRole = string; 

  