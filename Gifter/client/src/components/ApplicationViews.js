import { Routes, Route, Navigate} from "react-router-dom";
import { PostForm } from "./PostForm.js";
import PostList from "./PostList";
// import PostDetails from "./PostDetails";

const ApplicationViews = () => {

return (
     <Routes>
     
        <Route path="/" element= {<PostList />} />
        
        <Route path="/posts/add" element={<PostForm />} />
        
        {/* <Route path="/posts/:id" element=TODO: Post Details Component /> */}
                
        <Route path="*" element={<p>Whoops, nothing here...</p>} />
     
     </Routes>
    
    )
  

};

export default ApplicationViews;
