import React from 'react'

/**
 * SearchBar component
 * 
 * This component renders a search bar form with a single input field and a submit button.
 * The search bar is a full-width container with a white background and a black text.
 * The search bar has a hover effect that changes the border color to primary.
 * The input field is a text input with a placeholder of "Search Here ..."
 * The submit button is a button with a search icon and a white background.
 * When the form is submitted, it does not do anything yet.
 * 
 * @returns {React.ReactNode} - The rendered search bar component.
 */
const SearchBar = () => {

  return (
    <div className="w-100 p-4 text-black bg-white  fd-hover-border-primary border border-1">
        <h5 className='fw-bold'>Search</h5>
        <form action="" method="post" className='w-100 d-flex'>
            <div className="w-75"><input type="text" name="searchquery" placeholder='Search Here ...' className='p-3 border border-1 rounded-0'/></div>
            <div className="w-25"><button type='submit' className='btn btn-outline-dark rounded-0 py-3 px-4'><i className="bi bi-search"></i></button></div>
        </form>
    </div>
  )
}

export default SearchBar