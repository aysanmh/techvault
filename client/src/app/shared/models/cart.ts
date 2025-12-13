import { nanoid } from 'nanoid';

export type CartType = {
  id: string;
  items: CartItemType[];
};

export type CartItemType = {
  deviceId: number;
  deviceName: string;
  price: number;
  imageUrl: string;
  quantity: number;
};

export class Cart implements CartType {
  id = nanoid();
  items: CartItemType[] = [];
}
