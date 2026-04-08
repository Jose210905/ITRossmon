import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CategoriaService } from '../../services/categoria';

@Component({
  selector: 'app-categorias',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './categorias.html'
})
export class CategoriasComponent implements OnInit {
  categorias: any[] = [];
  mostrarFormulario = false;
  error = '';
  mensaje = '';

  form = { nombre: '', descripcion: '' };

  constructor(private categoriaService: CategoriaService) {}

  ngOnInit() {
    this.cargarCategorias();
  }

  cargarCategorias() {
    this.categoriaService.obtenerTodas().subscribe({
      next: (data) => this.categorias = data,
      error: () => this.error = 'Error al cargar categorías'
    });
  }

  abrirFormulario() {
    this.form = { nombre: '', descripcion: '' };
    this.mostrarFormulario = true;
    this.mensaje = '';
    this.error = '';
  }

  guardar() {
    if (!this.form.nombre) {
      this.error = 'El nombre es requerido';
      return;
    }
    this.categoriaService.insertar(this.form).subscribe({
      next: () => {
        this.mensaje = 'Categoría creada correctamente';
        this.mostrarFormulario = false;
        this.cargarCategorias();
      },
      error: () => this.error = 'Error al crear categoría'
    });
  }

  cancelar() {
    this.mostrarFormulario = false;
    this.error = '';
  }
}