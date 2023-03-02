import React, {useEffect, useState} from 'react';
import {Comment} from "../Comment/Comment";
import {IComment} from "../../interfaces/IComment";
import {Pagination, PaginationItem, PaginationLink} from "reactstrap";
import {useParams} from "react-router-dom";
import axios from "axios";

export let Home = () =>  {
    const [state, setState] = useState({
        comments: [] as IComment[],
        loading: true
    })
    
    const [pagesNumber, setPagesNumber] = useState(1);
    
    useEffect(() => {
        (async () => {
            const pagesNumberResponse = await axios.get("api/comments/GetCommentsPagesNumber");
            const pagesNumberData = await pagesNumberResponse.data;
            
            setPagesNumber(pagesNumberData);
        })()
    }, [])
    
    useEffect(() => {
        (async () => {
            const data = await axios.get<IComment[]>("api/comments", {
                params: {
                    skip: (currentPage - 1) * 25
                }
            });

            setState({
                comments: data.data,
                loading: false
            });
        })()
    }, [pagesNumber])

    const params = useParams();

    const currentPage = parseInt(!!params.pageNumber ? params.pageNumber : "1");
    const previousPage = currentPage > 1 ? currentPage - 1 : currentPage;
    const nextPage = currentPage < pagesNumber ? currentPage + 1 : currentPage;
    const pages = [...Array(pagesNumber).keys()].map(i => i + 1);
    //Add pages to show array

    return state.loading
        ? <p><em>Загрузка...</em></p>
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
                <PaginationItem disabled={currentPage == previousPage}>
                    <PaginationLink previous href={`./page/${previousPage}`} />
                </PaginationItem>
                {
                    pages.map(page => {
                        return <PaginationItem active={currentPage == page} key={page}>
                            <PaginationLink href={`./page/${page}`}>
                                {page}
                            </PaginationLink>
                        </PaginationItem>
                    })   
                }
                <PaginationItem disabled={currentPage == nextPage}>
                    <PaginationLink next href={`./page/${nextPage}`} />
                </PaginationItem>
            </Pagination>
        </>;
}