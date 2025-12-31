import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AccountService } from '../services/AccountService';
import { SnackbarService } from '../services/SnackbarService';

export const adminGuard: CanActivateFn = (route, state) => {
  const accountService = inject(AccountService);
  const router = inject(Router);
  const snack = inject(SnackbarService);

  if(accountService.isAdmin()){
    return true;    
  }
  else{
    snack.error('Unauthorized');
    router.navigateByUrl('/shop');
    return false;
  }
  
};
