import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NavbarService {

  private loggedIn: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

  constructor() { }

  get navState() {
    return this.loggedIn.asObservable();
  }

  hideNav() {
    this.loggedIn.next(false);
  }

  showNav() {
    this.loggedIn.next(true);
  }
}
