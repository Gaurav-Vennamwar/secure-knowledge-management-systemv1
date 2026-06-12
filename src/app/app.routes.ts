import { Routes } from '@angular/router';
import { CategoryList } from './Features/Category/category-list/category-list';
import { AddCategory } from './Features/Category/add-category/add-category';
import { EditCategory } from './Features/Category/edit-category/edit-category';
import { BlogpostList } from './Features/BlogPosts/blogpost-list/blogpost-list';
import { AddBlogpost } from './Features/BlogPosts/add-blogpost/add-blogpost';
import { EditBlogpost } from './Features/BlogPosts/edit-blogpost/edit-blogpost';
import { Home } from './Features/Public/home/home';
import { BlogDetails } from './Features/Public/blog-details/blog-details';
import { Login } from './Features/Auth/login/login';
import { adminGuard } from './Features/Auth/guards/admin-guard';
import { Register } from './Features/Auth/register/register/register';

export const routes: Routes = [
  {
    path: '', //blank to display it to home
    component: Home,
  },
  {
    path: 'blog/:url',
    component: BlogDetails,
  },
  {
    path: 'login',
    component: Login,
  },
  {
    path : 'register',
    component : Register
  },

  {
    path: 'admin/categories',
    component: CategoryList,
    canActivate: [adminGuard],
  },
  {
    path: 'admin/categories/add',
    component: AddCategory,
    canActivate: [adminGuard],
  },
  {
    path: 'admin/categories/edit/:id',
    component: EditCategory,
    canActivate: [adminGuard],
  },
  {
    path: 'admin/blogposts',
    component: BlogpostList,
    canActivate: [adminGuard],
  },
  {
    path: 'admin/blogposts/add',
    component: AddBlogpost,
    canActivate: [adminGuard],
  },
  {
    path: 'admin/blogposts/edit/:id',
    component: EditBlogpost,
    canActivate: [adminGuard],
  },
];
