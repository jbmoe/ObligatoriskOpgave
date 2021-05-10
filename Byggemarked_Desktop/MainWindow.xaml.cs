using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ByggemarkedLibrary.Controllers;
using ByggemarkedLibrary.Model;

namespace ByggemarkedLibrary
{
    public partial class MainWindow : Window
    {
        Controller controller;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnFetchBookings_Click(object sender, RoutedEventArgs e)
        {
            List<Booking> bookings;
            try
            {
                bookings = controller.FindBookingsForCustomer(tbxEmail.Text);
                bookings.Sort((a, b) => b.StartDate.CompareTo(a.StartDate));
                bookingsDataGrid.DataContext = bookings;
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            controller = Controller.GetInstance();
        }

        private void bookingsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var booking = bookingsDataGrid.SelectedItem;
            if (booking != null && booking is Booking b)
            {
                bookingDetailsGrid.DataContext = b;
                stateComboBox.SelectedItem = b.State;
            }
        }

        private void btnUpdateBooking_Click(object sender, RoutedEventArgs e)
        {
            Booking booking = (Booking)bookingDetailsGrid.DataContext;
            State state = (State)stateComboBox.SelectedItem;
            controller.SetStateForBooking(booking.BookingId, state);
        }

        private void btnDeleteBooking_Click(object sender, RoutedEventArgs e)
        {
            Booking booking = (Booking)bookingDetailsGrid.DataContext;
            controller.DeleteBooking(booking.BookingId);
        }
    }
}
