import { computed, inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Cart } from '../../shared/models/cart';
import { Device } from '../../shared/models/device';
import { CartItemType } from '../../shared/models/cart';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CartService {
  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);
  cart = signal<Cart | null>(null);
  itemCount = computed(() => this.cart()?.items.reduce((sum, item) => sum + item.quantity, 0));

  totals = computed(() => {
    const cart = this.cart();

    if (!cart) return null;

    const subtotal = cart.items.reduce((sum, item) => sum + item.price * item.quantity, 0);
    const shipping = 0;
    const discount = 0;

    return {
      subtotal,
      shipping,
      discount,
      total: subtotal + shipping - discount,
    };
  });

  getCart(id: string) {
    return this.http.get<Cart>(this.baseUrl + 'cart?id=' + id).pipe(
      map((cart) => {
        this.cart.set(cart);
        return cart;
      })
    );
  }

  setCart(cart: Cart) {
    return this.http.post<Cart>(this.baseUrl + 'cart', cart).subscribe({
      next: (cart) => this.cart.set(cart),
    });
  }

  addItemToCart(item: CartItemType | Device, quantity = 1) {
    const cart = this.cart() ?? this.createCart();
    if (this.isDevice(item)) {
      item = this.mapDeviceToCartItem(item);
    }
    cart.items = this.addOrUpdateItem(cart.items, item, quantity);
    this.cart.set(cart);
    this.setCart(cart);
  }

  removeItemFromCart(deviceId: number, quantity = 1) {
    const cart = this.cart();
    if (!cart) return;

    const index = cart.items.findIndex((x) => x.deviceId === deviceId);

    if (index !== -1) {
      if (cart.items[index].quantity > quantity) {
        cart.items[index].quantity -= quantity;
      } else {
        cart.items.splice(index, 1);
      }
      if (cart.items.length === 0) {
        this.deleteCart();
      } else {
        this.setCart(cart);
      }
    }
  }
  deleteCart() {
    this.http.delete(this.baseUrl + 'cart?id=' + this.cart()?.id).subscribe({
      next: () => {
        localStorage.removeItem('cart_id');
        this.cart.set(null);
      },
    });
  }

  private addOrUpdateItem(
    items: CartItemType[],
    item: CartItemType,
    quantity: number
  ): CartItemType[] {
    const existingItem = items.find((x) => x.deviceId === item.deviceId);
    if (!existingItem) {
      items.push({ ...item, quantity });
    } else {
      existingItem.quantity += quantity;
    }
    return items;
  }

  private mapDeviceToCartItem(item: Device): CartItemType {
    return {
      deviceId: item.id,
      deviceName: item.model,
      price: item.price,
      imageUrl: item.imageUrl,
      quantity: 1,
    };
  }

  private isDevice(item: CartItemType | Device): item is Device {
    return (item as Device).id !== undefined;
  }

  private createCart(): Cart {
    const cart = new Cart();
    localStorage.setItem('cart_id', cart.id);
    this.cart.set(cart);
    return cart;
  }
}
