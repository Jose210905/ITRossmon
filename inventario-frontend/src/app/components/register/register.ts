import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, RouterLink, CommonModule],
  templateUrl: './register.html'
})
export class RegisterComponent {
  username = '';
  password = '';
  mensaje = '';
  error = '';

  constructor(private authService: AuthService, private router: Router) {}

  register() {
    if (!this.username || !this.password) {
      this.error = 'Por favor completá todos los campos';
      return;
    }
    this.authService.register(this.username, this.password).subscribe({
      next: () => {
        this.mensaje = 'Usuario registrado correctamente';
        setTimeout(() => this.router.navigate(['/login']), 1500);
      },
      error: () => { this.error = 'El usuario ya existe'; }
    });
  }
}