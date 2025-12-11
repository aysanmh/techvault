import { Component, inject, OnInit } from '@angular/core';
import { Header } from "./layout/header/header";
import { Shop } from "./features/shop/shop";
import { RouterOutlet } from '@angular/router';


@Component({
  selector: 'app-root',
  standalone: true, 
  imports: [RouterOutlet,Header, Shop],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App  {
    title = 'TechVault'
}
