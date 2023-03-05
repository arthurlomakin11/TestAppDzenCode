import React, {useEffect, useState} from 'react';
import HomeStyles from "../Home/Home.module.scss";
import {Comment} from "../Comment/Comment";
import {CommentsHeader} from "../Comment/CommentsHeader";
import {IComment} from "../../interfaces/IComment";
import {useParams} from "react-router-dom";
import axios from "axios";
import {ReplyFormCommentIdContext} from "../CommentReplyForm/ReplyFormContext";

export let CommentPage = () =>  {
    const [state, setState] = useState({
        comments: [] as IComment[],
        loading: true
    })

    const params = useParams();

    let commentId:number;
    if(!!params.commentId) {
        commentId = parseInt(params.commentId);
    }
    else {
        commentId = -1
    }

    useEffect(() => {
        (async () => {
            if(commentId !== -1) {
                const response = await axios.get<IComment[]>("api/comment", {
                    params: {
                        Id: commentId
                    }
                });
                const data = response.data;
                setState({comments: data, loading: false});
            }
        })()
    }, [])
    
    const [replyFormCommentId, setReplyFormCommentId] = useState(0);

    return state.loading
        ? <p><em>Loading...</em></p>
        : <div>
            <div className={HomeStyles.CommentsSection}>
                <CommentsHeader/>

                <ReplyFormCommentIdContext.Provider value={replyFormCommentId}>
                    <ul className={HomeStyles.ContentListComments}>
                        {
                            state.comments.map(comment => {
                                return <li className={HomeStyles.ContentListItem} key={comment.Id}>
                                    <Comment comment={comment} onReplyOpenButtonClick={(commentId) => setReplyFormCommentId(commentId)}/>
                                </li>
                            })
                        }
                    </ul>
                </ReplyFormCommentIdContext.Provider>
            </div>
        </div>;
}