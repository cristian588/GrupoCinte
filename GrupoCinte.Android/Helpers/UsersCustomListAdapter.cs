using Android.Views;
using Android.Widget;
using GrupoCinte.Common.Dtos;
using System.Collections.Generic;

namespace GrupoCinte.Android.Helpers
{
    class UsersCustomListAdapter : BaseAdapter<UserForListDto>
    {
        List<UserForListDto> users;
        public UsersCustomListAdapter(List<UserForListDto> users)
        {
            this.users = users;
        }
        public override UserForListDto this[int position]
        {
            get
            {
                return users[position];
            }
        }
        public override int Count
        {
            get
            {
                return users.Count;
            }
        }
        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;

            if (view == null)
            {
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Xml.user_item_card, parent, false);

                var name = view.FindViewById<TextView>(Resource.Id.titleTextView);
                var description = view.FindViewById<TextView>(Resource.Id.descriptionTextView);
                var email = view.FindViewById<TextView>(Resource.Id.emailTextView);

                view.Tag = new UsersViewHolder() { Name = name, Description = description, Email = email };
            }

            var holder = (UsersViewHolder)view.Tag;
            holder.Id = users[position].Id;
            holder.Name.Text = users[position].Name + " " + users[position].LastName;
            holder.Description.Text = users[position].IdNumber;
            return view;
        }
    }

    public class UsersViewHolder : Java.Lang.Object
    {
        public TextView Name { get; set; }
        public TextView Description { get; set; }
        public TextView Email { get; set; }
        public int Id { get; set; }
    }
}