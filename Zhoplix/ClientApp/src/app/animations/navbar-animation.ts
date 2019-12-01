import { trigger, state, style, transition, animate } from "@angular/animations";

export const NavbarAnimation =
  trigger('navbar', [
    state('black', style({
      background: 'black'
    })),
    state('transparent', style({
      background: 'transparent'
    })),
    transition('black=>transparent', animate('300ms')),
    transition('transparent=>black', animate('300ms'))
  ])
