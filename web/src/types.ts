export interface Review {
    _id: string;
    rating: number;
    description: string;
    productId: string;
}

export interface Product {
  _id: string;
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
    _id: string;
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
    deliveryStatus: string;
    totalPrice: number;
}



export interface UserType {
  _id: string;
  firstName?: string;
  lastName?: string;
  email?: string;
  password?: string;
  roles?: UserRole[];
  isVerified?: boolean;
  isApproved?: boolean;
  rating?: number;
}

type UserRole = string; 

  