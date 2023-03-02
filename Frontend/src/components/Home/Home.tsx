import React, {useEffect, useState} from 'react';
import {Comment} from "../Comment/Comment";
import {IComment} from "../../interfaces/IComment";
import {Pagination, PaginationItem, PaginationLink} from "reactstrap";
import {useParams} from "react-router-dom";

export let Home = () =>  {
    const [state, setState] = useState({
        comments: [] as IComment[],
        loading: true
    })
    
    useEffect(() => {
        (async () => {
            const response = await fetch("api/comments");
            const data:IComment[] = await response.json();
            setState({comments: data, loading: false});
        })()
    }, [])

    const params = useParams();
    console.log(params);

    return state.loading
        ? <p><em>Loading...</em></p>
        : <>
            <table className="table table-striped">
                <thead>
                    <tr>
                        <th>Имя</th>
                        <th>Email</th>
                        <th>Дата создания</th>
                        <th>Комментарий</th>
                    </tr>
                </thead>
                <tbody>
                    {
                        state.comments.map(comment => {
                            return <tr key={comment.Id}>
                                <td>{comment.UserName}</td>
                                <td>{comment.Email}</td>
                                <td>{comment.DateAdded}</td>
                                <td><Comment comment={comment} showReplyButton={false}/></td>
                            </tr>
                        })
                    }
                </tbody>
            </table>
            <Pagination>
                <PaginationItem>
                    <PaginationLink previous href="#" />
                </PaginationItem>
                <PaginationItem>
                    <PaginationLink href="#">
                        1
                    </PaginationLink>
                </PaginationItem>
                <PaginationItem>
                    <PaginationLink href="#">
                        2
                    </PaginationLink>
                </PaginationItem>
                <PaginationItem>
                    <PaginationLink href="#">
                        3
                    </PaginationLink>
                </PaginationItem>
                <PaginationItem>
                    <PaginationLink href="#">
                        4
                    </PaginationLink>
                </PaginationItem>
                <PaginationItem>
                    <PaginationLink href="#">
                        5
                    </PaginationLink>
                </PaginationItem>
                <PaginationItem>
                    <PaginationLink next href="#" />
                </PaginationItem>
            </Pagination>
        </>;
}