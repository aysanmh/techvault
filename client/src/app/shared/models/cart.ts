import { nanoid } from 'nanoid';

export type CartType = {
  id: string;
  items: CartItemType[];
  deliveryMethodId?: number;
  paymentIntentId?: string,
  clientSecret?: string
};

export type CartItemType = {
  deviceId: number;
  deviceName: string;
  price: number;
  imageUrl: string;
  quantity: number;
};

export class Cart implements CartType {
  deliveryMethodId?: number;
  paymentIntentId?: string;
  clientSecret?: string;
  id = nanoid();
  items: CartItemType[] = [];
}
