import { inject } from '@angular/core';
import { Router, CanActivateFn } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const roleGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.hasPrivilegedRole()) {
    return true;
  }

  // Redirect to unauthorized page if role is insufficient
  router.navigate(['/unauthorized']);
  return false;
};
