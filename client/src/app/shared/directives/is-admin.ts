import { Directive, effect, inject, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { AccountService } from '../../core/services/AccountService';

@Directive({
  selector: '[appIsAdmin]',
})
export class IsAdmin {

  private accountService = inject(AccountService);

  private viewContainerRef = inject(ViewContainerRef);

  private templateRef = inject(TemplateRef);


  constructor() { 
    effect(()=> {
      const isAdmin = this.accountService.isAdmin();

      this.viewContainerRef.clear();

      if(isAdmin){
        this.viewContainerRef.createEmbeddedView(this.templateRef);
      }
    })
  }

}
