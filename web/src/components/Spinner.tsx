import React from 'react'
import { InfinitySpin, ThreeDots } from 'react-loader-spinner'

/**
 * Spinner component
 * 
 * This component renders a spinner component with the following props:
 * - className: mt-2 w-25 mx-auto
 * - children: ThreeDots loader
 * 
 * The component also renders an InfinitySpin spinner if the className is changed to 'w-100'
 * 
 * @returns {React.ReactNode} - a React node that renders the spinner component
 */
const Spinner = () => {
  return (
    <div className='mt-2 w-25 mx-auto'><ThreeDots /></div>
    // <InfinitySpin width='150' color='#fff'/>
  )
}

export default Spinner