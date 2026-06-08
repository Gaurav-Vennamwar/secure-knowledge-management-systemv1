import { Routes } from '@angular/router';
import { CategoryList } from './Features/Category/category-list/category-list';
import { AddCategory } from './Features/Category/add-category/add-category';
import { EditCategory } from './Features/Category/edit-category/edit-category';
import { BlogpostList } from './Features/BlogPosts/blogpost-list/blogpost-list';
import { AddBlogpost } from './Features/BlogPosts/add-blogpost/add-blogpost';
import { EditBlogpost } from './Features/BlogPosts/edit-blogpost/edit-blogpost';
import { Home } from './Features/Public/home/home';
import { BlogDetails } from './Features/Public/blog-details/blog-details';

export const routes: Routes = [

    {
        path : '',//blank to display it to home
        component : Home
    },
    {
        path : 'blog/:url',
        component : BlogDetails
    },

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
    },
    {
       path: 'admin/blogposts/edit/:id',
        component : EditBlogpost
    }
];
