export interface Review {
    _id:string;
    rating: number;
    description: string;
    productId: string;
}

export interface Product {
  _id:string;
  vendorId: string;
  name: string;
  description: string;
  category: string;
  price: number;
  isActive?: boolean;
  countInStock: number;
  lowStockThreshold: number;
  imageUrl: string;
}

export interface Order {
    _id: string;
    userId: string;
    createdBy: string;
    items: {
      [productId: string]: {
        price: number;
        qty: number;
      };
    };
    totalPrice: number;
    deliveryStatus: string;
}



export interface UserType {
  _id:string;
  firstName?: string;
  lastName?: string;
  email?: string;
  region?: string;
  country?: string;
  password?: string;
  roles?: UserRole[];
  isAuthorized?: boolean;
}

type UserRole = string; 

  