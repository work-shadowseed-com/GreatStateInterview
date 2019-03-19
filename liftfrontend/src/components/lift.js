import React, { Component } from 'react';

class Lift extends Component {

    render() {
        
        return (
            <div className="lift">
                <h2>Lift</h2>
                <p>Floor: {this.props.floor}</p>
                <p>Status: {this.props.status}</p>
            </div>
        );
    }
}

export default Lift;