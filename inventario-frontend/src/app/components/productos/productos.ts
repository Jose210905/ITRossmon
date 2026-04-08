import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ProductoService } from '../../services/producto';
import { CategoriaService } from '../../services/categoria';

@Component({
  selector: 'app-productos',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './productos.html'
})
export class ProductosComponent implements OnInit {
  productos: any[] = [];
  categorias: any[] = [];
  mostrarFormulario = false;
  editando = false;
  error = '';
  mensaje = '';

  form = { id: 0, nombre: '', descripcion: '', precio: 0, stock: 0, categoriaId: 0 };

  constructor(private productoService: ProductoService, private categoriaService: CategoriaService) {}

  ngOnInit() {
    this.cargarProductos();
    this.cargarCategorias();
  }

  cargarProductos() {
    this.productoService.obtenerTodos().subscribe({
      next: (data) => this.productos = data,
      error: () => this.error = 'Error al cargar productos'
    });
  }

  cargarCategorias() {
    this.categoriaService.obtenerTodas().subscribe({
      next: (data) => this.categorias = data,
      error: () => {}
    });
  }

  abrirFormulario() {
    this.form = { id: 0, nombre: '', descripcion: '', precio: 0, stock: 0, categoriaId: 0 };
    this.editando = false;
    this.mostrarFormulario = true;
    this.mensaje = '';
    this.error = '';
  }

  editar(p: any) {
    this.form = { id: p.id, nombre: p.nombre, descripcion: p.descripcion, precio: p.precio, stock: p.stock, categoriaId: p.categoriaId };
    this.editando = true;
    this.mostrarFormulario = true;
    this.mensaje = '';
    this.error = '';
  }

  guardar() {
    if (!this.form.nombre || this.form.precio <= 0 || this.form.stock < 0 || !this.form.categoriaId) {
      this.error = 'Completá todos los campos correctamente';
      return;
    }
    const datos = { nombre: this.form.nombre, descripcion: this.form.descripcion, precio: this.form.precio, stock: this.form.stock, categoriaId: this.form.categoriaId };

    if (this.editando) {
      this.productoService.actualizar(this.form.id, datos).subscribe({
        next: () => { this.mensaje = 'Producto actualizado'; this.mostrarFormulario = false; this.cargarProductos(); },
        error: () => this.error = 'Error al actualizar'
      });
    } else {
      this.productoService.insertar(datos).subscribe({
        next: () => { this.mensaje = 'Producto creado'; this.mostrarFormulario = false; this.cargarProductos(); },
        error: () => this.error = 'Error al crear producto'
      });
    }
  }

  eliminar(id: number) {
    if (!confirm('¿Eliminár este producto?')) return;
    this.productoService.eliminar(id).subscribe({
      next: () => { this.mensaje = 'Producto eliminado'; this.cargarProductos(); },
      error: () => this.error = 'Error al eliminar'
    });
  }

  cancelar() {
    this.mostrarFormulario = false;
    this.error = '';
  }
}