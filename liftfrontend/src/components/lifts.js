import React, { Component } from 'react';
import Lift from './lift';

class Lifts extends Component {

    constructor(props) {
        super(props);
        this.state = {
            liftData: {

            }
        }

        this.getLiftData = this.getLiftData.bind(this);
    }

    componentDidMount() {
        this.getLiftData();
        setInterval(() => this.getLiftData(), 1000);
    }

    getLiftData() {
        fetch("https://localhost:5001/api/call").then((response) => {
            response.json().then((data) => {
                this.setState({
                    liftData: data
                });
            });
        });
    }

    render() {

        if (this.state.liftData.length > 0) {
            return (
                <div>
                    {this.state.liftData.map(function(item, i) {
                        return <Lift floor={item.floor} status={item.status} key={i} />
                    })}
                </div>
            )
        } else {
            return (
                <p>Waiting for lift data...</p>
            )
        }
    }
}

export default Lifts;