import Styles from "./CommentReplyForm.module.scss";
import React, {useContext, useState} from "react";
import {ReplyFormCommentIdContext} from "./ReplyFormContext";

export let CommentReplyForm = ({id}:{id:number}) => {
    const [message, setMessage] = useState('');
    const ReplyFormCommentId:number = useContext(ReplyFormCommentIdContext); 
        
    const handleMessageChange = (event: { target: { value: React.SetStateAction<string>; }; }) => {
        setMessage(event.target.value);
    };
    
    function ToolbarElementClick() {
        /*setMessage()*/
    }
    
    return ReplyFormCommentId == id ? <div className={Styles.CommentReplyForm}>
        <ul>
            <li><button onClick={ToolbarElementClick}>a</button></li>
            <li>i</li>
            <li>strong</li>
            <li>code</li>
        </ul>
        <textarea value={message} onChange={handleMessageChange}/>
    </div> : <></>
}