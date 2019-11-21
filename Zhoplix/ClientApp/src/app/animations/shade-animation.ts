import { trigger, state, style, transition, animate } from "@angular/animations";

export const ShadeAnimation =
  trigger('shade', [
    state('visible', style({
      opacity: 1
    })),
    state('invisible', style({
      opacity: 0
    })),
    transition('visible=>invisible', animate('500ms')),
    transition('invisible=>visible', animate('100ms'))
  ])
