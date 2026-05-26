import { Routes } from '@angular/router';
import { CategoryList } from './Features/Category/category-list/category-list';
import { AddCategory } from './Features/Category/add-category/add-category';
import { EditCategory } from './Features/Category/edit-category/edit-category';
import { BlogpostList } from './Features/BlogPosts/blogpost-list/blogpost-list';
import { AddBlogpost } from './Features/BlogPosts/add-blogpost/add-blogpost';

export const routes: Routes = [

    {
        path : 'admin/categories',
        component : CategoryList
    },
    {
        path : 'admin/categories/add',
        component : AddCategory
    },
    {
        path : 'admin/categories/edit/:id',
        component : EditCategory

    },
    {
        path : 'admin/blogposts',
        component : BlogpostList
    },
    {
        path : 'admin/blogposts/add',
        component : AddBlogpost
    }
];
